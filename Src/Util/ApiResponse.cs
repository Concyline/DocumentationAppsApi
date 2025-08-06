namespace DocumentationAppsApi.Src.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public T? Data { get; set; }

        // ✅ 200 OK
        public static ApiResponse<T> Ok(T data, string message = "Operação realizada com sucesso", int statusCode = 200)
            => new ApiResponse<T> { Success = true, Message = message, StatusCode = statusCode, Data = data };

        // ✅ 201 Created
        public static ApiResponse<T> Created(T data, string message = "Recurso criado com sucesso")
            => new ApiResponse<T> { Success = true, Message = message, StatusCode = 201, Data = data };

        // ✅ 204 No Content (sem dados, mas operação bem-sucedida)
        public static ApiResponse<T> NoContent(string message = "Sem conteúdo")
            => new ApiResponse<T> { Success = true, Message = message, StatusCode = 204, Data = default };

        // ✅ 400 Bad Request
        public static ApiResponse<T> BadRequest(string message = "Requisição inválida")
            => new ApiResponse<T> { Success = false, Message = message, StatusCode = 400 };

        public static ApiResponse<T> Unauthorized(string message = "Não autorizado")
     => new ApiResponse<T> { Success = false, Message = message, StatusCode = 401 };

        // ✅ 403 Forbidden
        public static ApiResponse<T> Forbidden(string message = "Acesso negado")
            => new ApiResponse<T> { Success = false, Message = message, StatusCode = 403 };

        // ✅ 404 Not Found
        public static ApiResponse<T> NotFound(string message = "Recurso não encontrado")
            => new ApiResponse<T> { Success = false, Message = message, StatusCode = 404 };

        // ✅ 409 Conflict
        public static ApiResponse<T> Conflict(string message = "Conflito de dados")
            => new ApiResponse<T> { Success = false, Message = message, StatusCode = 409 };

        // ✅ 500 Internal Server Error
        public static ApiResponse<T> Error(string message = "Erro interno no servidor")
            => new ApiResponse<T> { Success = false, Message = message, StatusCode = 500 };

        // ✅ genérico para falhas com código customizável
        public static ApiResponse<T> Fail(string message, int statusCode = 400)
            => new ApiResponse<T> { Success = false, Message = message, StatusCode = statusCode };
    }
}
