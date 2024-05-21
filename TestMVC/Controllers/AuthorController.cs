using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestMVC.DTOs;
using TestMVC.Models;

namespace TestMVC.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;

        public AuthorController(IMapper mapper, AppDbContext dbcontext ) 
        {
            _mapper = mapper;
            _dbContext = dbcontext;
        }
        public async Task<IActionResult> Index()
        {

            List<AuthorGetDTO> authorDTOs = new List<AuthorGetDTO>();

            ResponseModel<List<AuthorModel>> responseModel = new ResponseModel<List<AuthorModel>>();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7130/Author");
                    HttpResponseMessage response = await client.GetAsync("");
                    response.EnsureSuccessStatusCode();

                    string responseData = await response.Content.ReadAsStringAsync();
                    responseModel = System.Text.Json.JsonSerializer.Deserialize<ResponseModel<List<AuthorModel>>>(responseData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    //using (JsonDocument doc = JsonDocument.Parse(responseData))
                    //{
                    //    JsonElement root = doc.RootElement;
                    //    JsonElement dataElement = root.GetProperty("data");//Gelen json "data" ve "message" propertylerinden ibaretdir, bize data lazim.
                    //    string rawData = dataElement.GetRawText();//stringe cevir

                    //    // Deserialize the data part into List<AuthorModel>
                    //    authorDTOs = System.Text.Json.JsonSerializer.Deserialize<List<AuthorGetDTO>>(rawData, new JsonSerializerOptions
                    //    {
                    //        PropertyNameCaseInsensitive = true
                    //    });

                    //    List<AuthorModel> authors = _mapper.Map<List<AuthorModel>>(authorDTOs);

                    //    responseModel.Data = authors;
                    //    responseModel.Success = true;
                    //}
                }
            }
            catch (Exception ex)
            {
                responseModel.Success = false;
                responseModel.Data = null;
            }
            return View(responseModel);
        }
        public async Task<IActionResult> AddAuthor()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAuthor(AuthorCreateDTO authorDTO)
        {
            try
            {
                AuthorModel authorModel = _mapper.Map<AuthorModel>(authorDTO);

                ResponseModel<AuthorModel> responseModel = new ResponseModel<AuthorModel>
                {
                    Data = authorModel
                };

                using (HttpClient client = new HttpClient())
                {
                    string jsonContent = JsonConvert.SerializeObject(responseModel);

                    // Create StringContent from JSON content
                    StringContent stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    client.BaseAddress = new Uri("https://localhost:7130/Author");

                    // Post the JSON content to the server
                    HttpResponseMessage responseMessage = await client.PostAsync("", stringContent);

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(); // Return the same view if there's an error
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<IActionResult> EditAuthor(int id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7130/");

                    HttpResponseMessage response = await client.GetAsync("Author/" + id.ToString());

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        using (JsonDocument doc = JsonDocument.Parse(responseData))
                        {
                            JsonElement root = doc.RootElement;
                            JsonElement dataElement = root.GetProperty("data"); // Assuming JSON structure has a "data" field
                            string rawData = dataElement.GetRawText(); // Convert "data" field to string

                            // Deserialize the data part into AuthorModel
                            AuthorGetDTO authorDTO = System.Text.Json.JsonSerializer.Deserialize<AuthorGetDTO>(rawData, new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });
                            AuthorModel author=_mapper.Map<AuthorModel>(authorDTO);
                            return View(author);
                        }
                    }
                    else
                    {
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur
                Console.WriteLine(ex.Message);
                return View("Error");
            }
        }



        [HttpPost]
        public async Task<IActionResult> EditAuthor(int id, AuthorCreateDTO authorDTO)
        {
            try
            {
                AuthorModel authorModel = _mapper.Map<AuthorModel>(authorDTO);

                authorModel.Id = id;

                ResponseModel<AuthorModel> responseModel = new ResponseModel<AuthorModel>
                {
                    Data = authorModel
                };

                using (HttpClient client = new HttpClient())
                {
                    string jsonContent = JsonConvert.SerializeObject(authorModel);
                    StringContent stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    client.BaseAddress = new Uri("https://localhost:7130/Author");

                    HttpResponseMessage responseMessage = await client.PutAsync("Author/" + id.ToString(), stringContent);

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(authorDTO); // Return the same view with errors if there's an error
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error");
            }
        }

        public async Task<IActionResult> DeleteAuthor()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7130/Author");

                    HttpResponseMessage responseMessage = await client.DeleteAsync("Author/" + id.ToString());

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(); 
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

    }

}
