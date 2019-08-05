using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EasyCode.Core.Utility
{
    /// <summary>
    /// 用于记录操作状态的信息对象,可以包含当前操作是否成功,如果不成功的原因,错误代码和原始异常.
    /// </summary>
    public class Result
    {

        public Result()
        {
        }
        /// <summary>
        /// 错误代码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public Result(int code, string message)
        {
            Code = code;
            Message = message;
        }
        /// <summary>
        /// 返回结果默认是成功(有效)
        /// </summary>
        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; } = true;

        /// <summary>
        /// 消息代码,默认0,可以自行定义
        /// </summary>
        [JsonProperty("code")]
        public int Code
        {
            get; set;
        }
        /// <summary>
        /// 消息内容
        /// </summary>
        [JsonProperty("message")]
        public string Message
        {
            get; set;
        }
        /// <summary>
        /// 失败结果
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        public void Failed(string message, int code = -1)
        {
            Succeeded = false;
            Code = code;
            Message = message;
            //ErrorMessages.Add(message);
        }
        /// <summary>
        /// 如果当前为错误结果时,提供原始异常
        /// </summary>
        [JsonIgnore]
        public Exception OriginalException
        {
            get; set;
        }
        /// <summary>
        /// 构建错误状态的对象
        /// </summary>
        /// <param name="code">错误代码</param>
        /// <param name="message">错误消息</param>
        /// <returns></returns>
        public static Result Fail(string message, int code = -1)
        {
            var result = new Result();
            result.Failed(message, code);
            return result;
        }

        public static Result<T> Fail<T>(string message, int code = -1, T defaultValue = default(T))
        {
            var result = new Result<T>();
            result.Failed(message, code);
            result.Object = defaultValue;
            return result;
        }

        public static Result Success(string message = null)
        {
            return new Result()
            {
                Message = message
            };
        }
        /// <summary>
        /// 构建返回成功结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Result<T> Success<T>(T value)
        {
            return new Result<T> { Object = value };
        }

        public void AddErrorMessage(string message)
        {
            Succeeded = false;
            ErrorMessages.Add(message);
        }


        [JsonIgnore]
        public List<string> ErrorMessages { get; } = new List<string>();
        [JsonProperty("data")]
        public virtual object Data
        {
            get; set;
        }
        public Result Merge(Result result)
        {
            if (Succeeded)
            {
                Succeeded = result.Succeeded;
            }
            this.ErrorMessages.AddRange(result.ErrorMessages);
            return this;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// 输出为错误信息,Json 字符串
        /// </summary>
        /// <returns></returns>
        public static string ToJsonString(this Result res)
        {
            return JsonConvert.SerializeObject(res, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
        /// <summary>
        /// 输出为错误信息,JsonResult 格式
        /// </summary>
        /// <returns></returns>
        public static JsonResult ToJsonResult(this Result res)
        {
            return new JsonResult(res, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
        /// <summary>
        /// 转换为分页后的Json数据
        /// </summary>
        /// <param name="res">操作结果</param>
        /// <param name="total">总记录数,如果为空则尝试判断从返回结果(IPagedList)中获取值</param>
        /// <param name="items">数据对象集合如果为空则使用,操作结果的返回值</param>
        /// <returns></returns>
        public static JsonResult ToPagedResult(this Result res, int? total = null, IEnumerable items = null)
        {

            if (res.Data is IPagedList)
            {
                var paged = res.Data as IPagedList;
                if (total == null)
                {
                    total = paged.TotalCount;
                }
                if (items == null)
                {
                    items = paged;
                }
            }
            return new JsonResult(new
            {
                total,
                items = items ?? res.Data ?? Enumerable.Empty<string>(),
                message = res.Message,
                errors = res.ErrorMessages == null || res.ErrorMessages.Count == 0 ? null : res.ErrorMessages,
                succeeded = res.Succeeded
            }, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
    /// <summary>
    /// 表示一个包含返回结果的对象
    /// </summary>
    /// <typeparam name="TResult">实际结果</typeparam>
    public class Result<TResult> : Result
    {
        /// <summary>
        /// 实际结果
        /// </summary>
        [JsonIgnore]
        public new TResult Object
        {
            get; set;
        }
        public new static Result<TResult> Success(string message = null)
        {
            return new Result<TResult>()
            {
                Message = message
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Object == null)
            {
                return string.Empty;
            }
            return Object.ToString();
        }
    }

}
