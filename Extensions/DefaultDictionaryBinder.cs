using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;

    /// <summary>
    /// ASP.NET MVC Default Dictionary Binder
    /// </summary>
    public class DefaultDictionaryBinder : DefaultModelBinder
    {
        IModelBinder nextBinder;

        /// <summary>
        /// Create an instance of DefaultDictionaryBinder.
        /// </summary>
        public DefaultDictionaryBinder() : this(null)
        {
        }

        /// <summary>
        /// Create an instance of DefaultDictionaryBinder.
        /// </summary>
        /// <param name="nextBinder">The next model binder to chain call. If null, by default, the DefaultModelBinder is called.</param>
        public DefaultDictionaryBinder(IModelBinder nextBinder)
        {
            this.nextBinder = nextBinder;
        }

        private IEnumerable<string> GetValueProviderKeys(ControllerContext context)
        {
#if !ASPNETMVC1
            List<string> keys = new List<string>();
            keys.AddRange(context.HttpContext.Request.Form.Keys.Cast<string>());
            keys.AddRange(((IDictionary<string, object>)context.RouteData.Values).Keys.Cast<string>());
            keys.AddRange(context.HttpContext.Request.QueryString.Keys.Cast<string>());
            keys.AddRange(context.HttpContext.Request.Files.Keys.Cast<string>());
            return keys;
#else
            return bindingContext.ValueProvider.Keys;
#endif
        }

        private object ConvertType(string stringValue, Type type)
        {
            return TypeDescriptor.GetConverter(type).ConvertFrom(stringValue);
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Type modelType = bindingContext.ModelType;
            Type idictType = modelType.GetInterface("System.Collections.Generic.IDictionary`2");
            if (idictType != null)
            {
                object result = null;

                Type[] ga = idictType.GetGenericArguments();
                IModelBinder valueBinder = Binders.GetBinder(ga[1]);
                
                foreach (string key in GetValueProviderKeys(controllerContext))
                {
                    if (key.StartsWith(bindingContext.ModelName + "[", StringComparison.InvariantCultureIgnoreCase))
                    {
                        int endbracket = key.IndexOf("]", bindingContext.ModelName.Length + 1);
                        if (endbracket == -1)
                            continue;

                        object dictKey;
                        try
                        {
                            dictKey = ConvertType(key.Substring(bindingContext.ModelName.Length + 1, endbracket - bindingContext.ModelName.Length - 1), ga[0]);
                        }
                        catch (NotSupportedException)
                        {
                            continue;
                        }

                        ModelBindingContext innerBindingContext = new ModelBindingContext()
                        {
#if ASPNETMVC1
                            Model = null,
                            ModelType = ga[1],
#else
                            ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => null, ga[1]),
#endif
                            ModelName = key.Substring(0, endbracket + 1),
                            ModelState = bindingContext.ModelState,
                            PropertyFilter = bindingContext.PropertyFilter,
                            ValueProvider = bindingContext.ValueProvider
                        };
                        object newPropertyValue = valueBinder.BindModel(controllerContext, innerBindingContext);

                        if (result == null)
                            result = CreateModel(controllerContext, bindingContext, modelType);

                        if (!(bool)idictType.GetMethod("ContainsKey").Invoke(result, new object[] { dictKey }))
                            idictType.GetProperty("Item").SetValue(result, newPropertyValue, new object[] { dictKey });
                    }
                }

                return result;
            }

            if (nextBinder != null)
            {
                return nextBinder.BindModel(controllerContext, bindingContext);
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }

