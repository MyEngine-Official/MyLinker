using MyLinker.Exceptions;
using System.Reflection;

namespace MyLinker.Linker
{
    public class Linker<EN, EX>
    {
        public static EX MapObjects(EN entryObject, EX exitObject, bool nullPropertySafety = true, params string[]? propertyOmit) => BaseMapObjects(entryObject, exitObject, nullPropertySafety, propertyOmit);
        private static EX BaseMapObjects(EN entryObject, EX exitObject, bool nullPropertySafety, string[]? propertyOmit = null)
        {
            if (entryObject == null) throw new ArgumentNullException(nameof(entryObject));
            if (exitObject == null) throw new ArgumentNullException(nameof(exitObject));

            try
            {
                PropertyInfo[] entryProperties = typeof(EN).GetProperties();
                var exitProperties = exitObject?.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

                foreach (PropertyInfo property in entryProperties)
                {
                    var TargetProperty = exitProperties!.FirstOrDefault(x => x.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase));
                    if (TargetProperty == null) continue;

                    if (property.GetValue(entryObject) == null && nullPropertySafety == true) continue;
                    if (propertyOmit != null && propertyOmit.Contains(TargetProperty.Name)) continue;

                    if (TargetProperty.PropertyType == property.PropertyType)
                    {
                        TargetProperty.SetValue(exitObject, property.GetValue(entryObject));

                    }else if (TargetProperty.PropertyType == typeof(string))
                    {
                        TargetProperty.SetValue(exitObject, property.GetValue(entryObject)!.ToString());
                    }else if(TargetProperty.PropertyType == typeof(bool) && property.PropertyType == typeof(string))
                    {
                        if (bool.TryParse((string)property.GetValue(entryObject), out bool boolExit)){
                            TargetProperty.SetValue(exitObject, boolExit);
                        }
                    }
                    else if(TargetProperty.PropertyType == typeof(bool) && property.PropertyType == typeof(int))
                    {
                        if((int)property.GetValue(entryObject)! == 1)
                        {
                            TargetProperty.SetValue(exitObject, true);
                        }
                        else if((int)property.GetValue(entryObject)! == 0)
                        {
                            TargetProperty.SetValue(exitObject, false);
                        }
                    }else if (TargetProperty.PropertyType == typeof(int))
                    {
                        int valor;
                        if (property.PropertyType == typeof(string))
                        {
                            if (int.TryParse((string)property.GetValue(entryObject)!, out int result))
                            {
                                valor = result;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if(property.PropertyType != typeof(bool))
                        {
                            valor = Convert.ToInt32(property.GetValue(entryObject));
                        }
                        else
                        {
                            if ((bool)property.GetValue(entryObject)! == true)
                            {
                                valor = 1;
                            }
                            else
                            {
                                valor = 0;
                            }

                            
                        }
                            TargetProperty.SetValue(exitObject, valor);
                    }
                }

                return exitObject;
            }
            catch (Exception ex)
            {
                throw new LinkerException("Hubo un error al mappear las propiedades", ex);
            }
        }
    }

    
}

