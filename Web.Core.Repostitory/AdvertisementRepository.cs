using System;
using Web.Core.IRepository;

namespace Web.Core.Repostitory
{
    public class AdvertisementRepository : IAdvertisementRepository
    {
       
        public int sum(int i, int j)
        {
            return i + j;
        }
    }
}
