﻿using NorthwindBackend.Core.Utilities.Results;

namespace NorthwindBackend.Core.Utilities.Business
{
    public class BusinessRules
    {
        public static IResult Run(params IResult[] logics)
        {
            foreach (var result in logics)
            {
                if (!result.IsSuccess)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
