//-----------------------------------------------------------------------
// <copyright file=" ProductStatus.cs" company="xxxx Enterprises">
// * Copyright (C) 2021 haha
// * version : 4.0.30319.42000
// * FileName: ProductStatus.cs
// * history : Created by T4 01/30/2021 13:09:48 
// * 
// </copyright>
//-----------------------------------------------------------------------

using System;
 
namespace Test.Entities
{
    /// <summary>
    /// ProductStatus Entity Model
    /// </summary>    
    public class ProductStatus
    {

        /// <summary>
        /// 产品ID
        /// </summary>
        [Dapper.Contrib.Extensions.Key]
        [Dapper.Contrib.Extensions.ExplicitKey]
        public int Id { get; set; }
        
        /// <summary>
        /// 产品状态
        /// </summary>
        public int Status { get; set; }
    }
}
