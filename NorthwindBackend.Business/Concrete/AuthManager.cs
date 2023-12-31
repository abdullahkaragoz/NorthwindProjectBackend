﻿using NorthwindBackend.Business.Abstract;
using NorthwindBackend.Business.Constants;
using NorthwindBackend.Core.Entities.Concrete;
using NorthwindBackend.Core.Utilities.Results;
using NorthwindBackend.Core.Utilities.Security.Hashing;
using NorthwindBackend.Core.Utilities.Security.Jwt;
using NorthwindBackend.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthwindBackend.Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var claims = _userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, claims);

            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }

        public IDataResult<User> Login(UserForLoginDto userForLogin)
        {
            var userToCheck = _userService.GetByMail(userForLogin.Email);
            if (userToCheck == null)
                return new ErrorDataResult<User>(Messages.UserNotFound);
            if (!HashingHelper.VerifyPasswordHash(userForLogin.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
                return new ErrorDataResult<User>(Messages.PasswordError);

            return new SuccessDataResult<User>(userToCheck, Messages.SuccessfulLogin);
        }

        public IDataResult<User> Register(UserForRegisterDto userForRegister, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var user = new User
            {
                Email = userForRegister.Email,
                FirstName = userForRegister.FirstName,
                LastName = userForRegister.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true

            };
            _userService.Add(user);

            return new SuccessDataResult<User>(user, Messages.UserRegistered);

        }

        public IDataResult<User> Register(UserForRegisterDto userForRegister)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userForRegister.Password, out passwordHash, out passwordSalt);

            var user = new User
            {
                Email = userForRegister.Email,
                FirstName = userForRegister.FirstName,
                LastName = userForRegister.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true,
            };
            _userService.Add(user);
            return new SuccessDataResult<User>(user, Messages.UserRegistered);
        }

        public IResult UserExists(string email)
        {
            if (_userService.GetByMail(email) != null)
                return new ErrorResult(Messages.UserAlreadyExist);
            return new SuccessResult();

        }
    }
}
