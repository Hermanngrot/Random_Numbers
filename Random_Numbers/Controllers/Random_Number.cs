using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Random_Numbers.Controllers
{
    // Ruta base para este controlador
    [Route("api/[controller]")]
    [ApiController]
    public class Random_NumberController : ControllerBase
    {
        private readonly Random _random = new Random();

        // GET api/random_number/ram
        [HttpGet("ram")]
        public IActionResult GetRandomNumber([FromQuery] int? min, [FromQuery] int? max)
        {
            if (min == null && max == null)
            {
                // Número entero cualquiera
                int value = _random.Next();
                return Ok(new { result = value });
            }

            if (min == null || max == null)
            {
                return BadRequest(new { error = "Debe proporcionar ambos parámetros 'min' y 'max' o ninguno." });
            }

            if (min > max)
            {
                return BadRequest(new { error = "'min' no puede ser mayor que 'max'." });
            }

            int result = _random.Next(min.Value, max.Value + 1);
            return Ok(new { result });
        }

        // GET api/random_number/decimal
        [HttpGet("decimal")]
        public IActionResult GetRandomDecimal()
        {
            double value = _random.NextDouble(); // entre 0 y 1
            return Ok(new { result = value });
        }

        // GET api/random_number/string?length=8
        [HttpGet("string")]
        public IActionResult GetRandomString([FromQuery] int length = 8)
        {
            if (length < 1 || length > 1024)
                return BadRequest(new { error = "'length' debe estar entre 1 y 1024." });

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string str = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());

            return Ok(new { result = str });
        }

        // POST api/random_number/custom
        [HttpPost("custom")]
        public IActionResult GetCustom([FromBody] CustomRandomRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Type))
                return BadRequest(new { error = "Debe indicar el campo 'type' ('number' | 'decimal' | 'string')." });

            switch (req.Type.ToLower())
            {
                case "number":
                    if (req.Min == null || req.Max == null)
                        return BadRequest(new { error = "Para 'number' debe enviar 'min' y 'max'." });
                    if (req.Min > req.Max)
                        return BadRequest(new { error = "'min' no puede ser mayor que 'max'." });

                    int num = _random.Next(req.Min.Value, req.Max.Value + 1);
                    return Ok(new { result = num });

                case "decimal":
                    int decimals = req.Decimals ?? 2;
                    double dec = Math.Round(_random.NextDouble(), decimals);
                    return Ok(new { result = dec });

                case "string":
                    int length = req.Length ?? 8;
                    if (length < 1 || length > 1024)
                        return BadRequest(new { error = "'length' debe estar entre 1 y 1024." });

                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    string str = new string(Enumerable.Repeat(chars, length)
                        .Select(s => s[_random.Next(s.Length)]).ToArray());
                    return Ok(new { result = str });

                default:
                    return BadRequest(new { error = "Tipo no válido. Use 'number' | 'decimal' | 'string'." });
            }
        }
    }

    // Modelo para el POST custom
    public class CustomRandomRequest
    {
        public string? Type { get; set; }
        public int? Min { get; set; }
        public int? Max { get; set; }
        public int? Decimals { get; set; }
        public int? Length { get; set; }
    }
}