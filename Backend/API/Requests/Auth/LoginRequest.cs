﻿namespace API.Requests.Auth
{
    public sealed record LoginRequest
       (string Email,
       string Password);
}
