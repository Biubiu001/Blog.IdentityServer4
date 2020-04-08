using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using Web.Core.Common.Helper;

namespace Web.Core.Common.Redis
{
    public class RedisCacheManage : IRedisCacheManage
    {
        public readonly string redisConnenctionString;
        public volatile ConnectionMultiplexer redisConnection;
        private readonly object redisConnectionLock = new object();

        public RedisCacheManage()
        {

            string redisConfigurataion = Appsettings.app(new string[] { "AppSettings", "RedisCachingAOP", "ConnectionString" });
            if (string.IsNullOrWhiteSpace(redisConfigurataion)) {

                throw new ArgumentException("redis config is empty", nameof(redisConfigurataion));
            
            }
            this.redisConnenctionString = redisConfigurataion;
            this.redisConnection = GetRedisConnection();
        }
        /// <summary>
        /// 核心代码，获取连接实例
        /// 通过双if 夹lock的方式，实现单例模式
        /// </summary>
        /// <returns></returns>
        private ConnectionMultiplexer GetRedisConnection()
        {
            //如果已经连接实例，直接返回
            if (this.redisConnection != null && this.redisConnection.IsConnected)
            {
                return this.redisConnection;
            }
            //加锁，防止异步编程中，出现单例无效的问题
            lock (redisConnectionLock)
            {
                if (this.redisConnection != null)
                {
                    //释放redis连接
                    this.redisConnection.Dispose();
                }
                try
                {
                    this.redisConnection = ConnectionMultiplexer.Connect(redisConnenctionString);
                }
                catch (Exception)
                {
                    //throw new Exception("Redis服务未启用，请开启该服务，并且请注意端口号，本项目使用的的6319，而且我的是没有设置密码。");
                }
            }
            return this.redisConnection;
        }
        public void Clear()
        {
            throw new NotImplementedException();
        }

        public TEntity Get<TEntity>(string key)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Get(string key)
        {
            return redisConnection.GetDatabase().KeyExists(key);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            return redisConnection.GetDatabase().StringGet(key);
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>

        public void Remove(string key)
        {
            redisConnection.GetDatabase().KeyDelete(key);
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheTime"></param>
        public void Set(string key, object value, TimeSpan cacheTime)
        {
            if (value != null)
            {
                //序列化，将object值生成RedisValue
                redisConnection.GetDatabase().StringSet(key, SerializeHelper.Serialize(value), cacheTime);
            }
        }
    }
}
