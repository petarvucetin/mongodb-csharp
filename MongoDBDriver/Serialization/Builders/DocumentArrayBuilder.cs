﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace MongoDB.Driver.Serialization.Builders
{
    /// <summary>
    /// 
    /// </summary>
    public class DocumentArrayBuilder : IObjectBuilder
    {
        private readonly List<object> _list = new List<object>();

        /// <summary>
        /// Completes this instance.
        /// </summary>
        /// <returns></returns>
        public object Complete(){
            var type = GetResultListType();

            if(type == typeof(object))
                return _list;

            var listType = typeof(List<>).MakeGenericType(type);

            var list = (IList)Activator.CreateInstance(listType);

            foreach(var obj in _list)
                list.Add(obj);

            return list;
        }

        /// <summary>
        /// Begins the property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Type BeginProperty(string name){
            return typeof(Document);
        }

        /// <summary>
        /// Ends the property.
        /// </summary>
        /// <param name="value">The value.</param>
        public void EndProperty(object value){
            _list.Add(value);
        }

        /// <summary>
        /// Gets the type of the result list.
        /// </summary>
        /// <returns></returns>
        private Type GetResultListType(){
            //Todo: compare the tree up instead only to object
            if(_list.Count == 0)
                return typeof(object);

            Type commonType = null;

            foreach(var obj in _list){
                if(obj==null)
                    continue;
                var objType = obj.GetType();
                if(commonType == null)
                    commonType = objType;
                else if(commonType != objType)
                    return typeof(object);
            }

            return commonType;
        }
    }
}