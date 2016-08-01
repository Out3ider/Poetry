using System;
using Sail.Common;

namespace Poetry.Model
{
    /// <summary>
    /// 奖品基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PrizeBase<T> : IModel where T : IModel
    {
       
    }
}