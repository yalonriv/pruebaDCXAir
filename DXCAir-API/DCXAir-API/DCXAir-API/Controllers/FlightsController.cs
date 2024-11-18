﻿using System.Collections;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using DCXAir_API.Models;
using Newtonsoft.Json;

namespace DCXAir_API.Controllers
{
    [Route("flights")]
    [ApiController]
    public class FlightsController : ControllerBase
    {

        // Lista simulada de vuelos (esto sería reemplazado por la lectura de un archivo o base de datos)
        private readonly List<Flight> Flights;

        // Constructor para cargar los vuelos desde un archivo o base de datos
        public FlightsController()
        {
            // Simulamos que cargamos los vuelos desde un archivo o una base de datos
            string miJson = System.IO.File.ReadAllText("markets.json");
            Flights = JsonConvert.DeserializeObject<List<Flight>>(miJson);
        }

        [HttpGet]
        [Route("getAllFlights")]
        public IActionResult GetFlights()
        {
            try
            {
                // Deserializamos el JSON en una lista de objetos Flight
                List<Flight> flights = Flights;
            
                // Retornar los vuelos como una respuesta JSON
                return Ok(flights);
            }
            catch (FileNotFoundException)
            {
                return NotFound("El archivo JSON no fue encontrado.");
            }
            catch (JsonException)
            {
                return BadRequest("Hubo un error al procesar el archivo JSON.");
            }
        }

        [HttpPost("filterByOneWay")]
        public IActionResult GetFlightsByOrigin([FromBody] FlightFilterDTO filter)
        {
            if (filter == null || string.IsNullOrEmpty(filter.Origin) || string.IsNullOrEmpty(filter.Destination))
            {
                return BadRequest("Debe proporcionar un origen válido.");
            }

            // Search One Way Flight
            var filteredOneWayFlights = Flights.Where(f =>
            f.Origin.ToLower().Contains(filter.Origin.ToLower()) &&
            f.Destination.ToLower().Contains(filter.Destination.ToLower())).ToList();

            if (filteredOneWayFlights.Count == 0)
            {
                return NotFound("There is no flights with the search criterial.");
            }
            return Ok(filteredOneWayFlights);
        }


    } 


}
