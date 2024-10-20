﻿using Microsoft.AspNetCore.Mvc;
using Application.Exceptions;
using Application.Interfaces;
using Application.Request;
using Application.Response;
using Microsoft.AspNetCore.Identity.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(typeof(ApiError), 400)]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var result = await _userService.GetUserById(id);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (ExceptionNotFound ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 400 };
            }
        }
        [HttpPost("create")]
        [ProducesResponseType(typeof(UserResponse), 201)]
        [ProducesResponseType(typeof(ApiError), 400)]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest request)
        {
            try
            {
                var result = await _userService.CreateUser(request);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (BadRequestException ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 400 };
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            try
            {
                // El método UserLogin ahora devuelve tanto el access token como el refresh token
                var tokenResponse = await _userService.UserLogin(request.Email, request.Password);
                if (tokenResponse == null)
                {
                    return Unauthorized(new { message = "Invalid credentials" });
                }

                // Retornar la respuesta con los tokens
                return Ok(tokenResponse);
            }
            catch (Exception ex)
            {
                // Puedes manejar otras excepciones si es necesario
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("profile")]
        [Authorize] // Requiere autenticación con token JWT
        [ProducesResponseType(typeof(UserPersonalInfoResponse), 200)]
        [ProducesResponseType(typeof(ApiError), 400)]
        public async Task<IActionResult> GetUserProfile()
        {
            try
            {
                // Extraer el ID del usuario autenticado del token JWT
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Obtener la información del usuario
                var result = await _userService.GetUserPersonalInfo(userId);
                return Ok(result);
            }
            catch (ExceptionNotFound ex)
            {
                return NotFound(new ApiError { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiError { Message = ex.Message });
            }
        }



        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            try
            {
                // Llama al servicio para refrescar el token
                var tokenResponse = await _userService.RefreshToken(request.AccessToken, request.RefreshToken);

                // En este punto, las excepciones de token inválido o expirado se manejan en el servicio
                return Ok(tokenResponse);
            }
            catch (UnauthorizedAccessException ex)
            {
                // Si hay un problema con la validez del refresh token o el access token
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Para cualquier otra excepción, devuelve un error 500
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var result = await _userService.ChangePassword(request);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpDelete]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(typeof(ApiError), 400)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUser(id);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (ExceptionNotFound ex)
            {

                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 400 };
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(typeof(ApiError), 400)]
        public async Task<IActionResult> UpdateUser(int userId, UserUpdateRequest request)
        {
            try
            {
                var result = await _userService.UpdateUser(userId, request);
                return new JsonResult(result) { StatusCode = 200 };
            }
            catch (ExceptionNotFound ex)
            {
                return new JsonResult(new ApiError { Message = ex.Message }) { StatusCode = 400 };
            }
        }
    }
}
