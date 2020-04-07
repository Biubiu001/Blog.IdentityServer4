using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Core.Common.MemoryCache;

namespace Web.Core.AOP
{
    public class BlogCacheAOP : CacheAOPbase
    {
        private readonly ICaching _cahche;
        public BlogCacheAOP(ICaching cache) {

            _cahche = cache;
        }
        public override void Intercept(IInvocation invocation)
        {
            //获取自定义缓存键
            var cacheKey = CustomCacheKey(invocation);
            //根据key获取相应的缓存值
            var cacheValue = _cahche.Get(cacheKey);
            if (cacheValue != null) {
                //将当前获取到的缓存值，赋值给当前执行方法
                invocation.ReturnValue = cacheValue;
                return;
            }
            //去执行当前的方法
            invocation.Proceed();
            //存入缓存
            if (!string.IsNullOrWhiteSpace(cacheKey)) {

                _cahche.Set(cacheKey,invocation.ReturnValue);
            }

        }
    }
}
