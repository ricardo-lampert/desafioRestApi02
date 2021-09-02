using System.Collections.Generic;
using FluentValidation.Results;

namespace RestApi.Models
{
    public interface IProductGateway
    {
        float GetPrice(string category, float cost);
    }
}
