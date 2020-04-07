using System;
using Web.Core.IRepository;
using Web.Core.IServices;
using Web.Core.Repostitory;

namespace Web.Core.Service
{
    public class AdvertisementServices: IAdvertisementService
    {
        IAdvertisementRepository dal = new AdvertisementRepository();

        public int Sum(int i, int j)
        {
           return  dal.sum(i,j);
        }
    }
}
