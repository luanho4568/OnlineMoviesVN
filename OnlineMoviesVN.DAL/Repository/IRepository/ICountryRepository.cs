﻿using OnlineMoviesVN.Database.Models;

namespace OnlineMoviesVN.DAL.Repository.IRepository
{
    public interface ICountryRepository : IRepository<Country>
    {
        void Update(Country country);
    }
}
