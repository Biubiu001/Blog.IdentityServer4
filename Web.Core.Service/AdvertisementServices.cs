using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Web.Core.IRepository;
using Web.Core.IServices;
using Web.Core.Model.Models;
using Web.Core.Service.Base;

namespace Web.Core.Service
{
    public class AdvertisementServices: BaseServices<Advertisement>, IAdvertisementService
    {
        //IAdvertisementRepository dal;
        //public AdvertisementServices(IAdvertisementRepository dal)
        //{
        //    this.dal = dal;
        //    base.baseDal = dal;
        //}
    }
}
