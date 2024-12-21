//-----------------------------------------------------------------
//    <copyright file="SessionManager.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>21-12-2024</date>
//    <time>17:15</time>
//    <version>1.0</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

namespace trabalho_oop.api.models;

public class LoginRequest
{
    public string Email { get; set; } // StaffCode or Email
    public string Password { get; set; }
}

public class RegisterRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}