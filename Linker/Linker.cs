using MyLinker.Exceptions;
using System.Reflection;

namespace MyLinker.Linker
{
    public class Linker<EN, EX>
    {
        public static EX MapObjects(EN entryObject, EX exitObject, bool nullPropertySafety, string[]? propertyOmit) => Linker<EN, EX>.BaseMapObjects(entryObject, exitObject, nullPropertySafety, propertyOmit);
        public static EX MapObjects(EN entryObject, EX exitObject, bool nullPropertySafety) => Linker<EN, EX>.BaseMapObjects(entryObject, exitObject, nullPropertySafety, null);
        public static EX MapObjects(EN entryObject, EX exitObject, string[] propertyOmit) => Linker<EN, EX>.BaseMapObjects(entryObject, exitObject, null, propertyOmit);
        public static EX MapObjects(EN entryObject, EX exitObject, string propertyOmit) => Linker<EN, EX>.BaseMapObjects(entryObject, exitObject, null, [propertyOmit]);
        public static EX MapObjects(EN entryObject, EX exitObject, string propertyOmit1, string propertyOmit2) => Linker<EN, EX>.BaseMapObjects(entryObject, exitObject, null, [propertyOmit1, propertyOmit2]);
        public static EX MapObjects(EN entryObject, EX exitObject, string propertyOmit1, string propertyOmit2, string propertyOmit3) => Linker<EN, EX>.BaseMapObjects(entryObject, exitObject, null, [propertyOmit1, propertyOmit2, propertyOmit3]);
        public static EX MapObjects(EN entryObject, EX exitObject, string propertyOmit1, string propertyOmit2, string propertyOmit3, string propertyOmit4) => Linker<EN, EX>.BaseMapObjects(entryObject, exitObject, null, [propertyOmit1, propertyOmit2, propertyOmit3, propertyOmit4]);
        public static EX MapObjects(EN entryObject, EX exitObject, string propertyOmit1, string propertyOmit2, string propertyOmit3, string propertyOmit4, string propertyOmit5) => Linker<EN, EX>.BaseMapObjects(entryObject, exitObject, null, [propertyOmit1, propertyOmit2, propertyOmit3, propertyOmit4, propertyOmit5]);
        public static EX MapObjects(EN entryObject, EX exitObject) => Linker<EN, EX>.BaseMapObjects(entryObject, exitObject, null, null);
        private static EX BaseMapObjects(EN entryObject, EX exitObject, bool? nullPropertySafety, string[]? propertyOmit)
        {
            try
            {
                PropertyInfo[] entryProperties = typeof(EN).GetProperties();

                foreach (PropertyInfo property in entryProperties)
                {
                    var TarjetProperty = exitObject?.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault(x => x.Name.ToLower() == property.Name.ToLower());
                    if (TarjetProperty == null) continue;
                    if (propertyOmit != null && propertyOmit.Contains(TarjetProperty.Name)) continue;
                    if (property.GetValue(entryObject) == null || TarjetProperty.GetValue(exitObject) == null) continue;

                    if (TarjetProperty.PropertyType == property.PropertyType)
                    {
                        TarjetProperty.SetValue(exitObject, property.GetValue(entryObject));

                    }else if (TarjetProperty.PropertyType == typeof(string))
                    {
                        TarjetProperty.SetValue(exitObject, property?.GetValue(entryObject)?.ToString());
                    }else if(TarjetProperty.PropertyType == typeof(bool) && property.PropertyType == typeof(string))
                    {
                        TarjetProperty.SetValue(exitObject, bool.Parse((string)property?.GetValue(entryObject)!));
                    }
                    else if(TarjetProperty.PropertyType == typeof(bool) && property.PropertyType == typeof(int))
                    {
                        if((int)property.GetValue(entryObject) == 1)
                        {
                            TarjetProperty.SetValue(exitObject, true);
                        }
                        else if((int)property.GetValue(entryObject) == 0)
                        {
                            TarjetProperty.SetValue(exitObject, false);
                        }
                    }else if (TarjetProperty.PropertyType == typeof(int))
                    {
                        int valor;
                        if (property.PropertyType == typeof(string))
                        {
                            _ = int.TryParse((string)property.GetValue(entryObject)!, out int result);
                            valor = (int)result;
                        }
                        else if(property.PropertyType != typeof(bool))
                        {
                            valor = (int)(property.GetValue(entryObject));
                        }
                        else
                        {
                            if((bool)property.GetValue(entryObject)! == true)
                            valor = 1;

                            valor = 0;
                        }
                            TarjetProperty.SetValue(exitObject, valor);
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

