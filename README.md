# MyLinker

`MyLinker` es una librería ligera en **C#** que permite mapear las propiedades de un objeto de entrada (**Entry Object**) hacia un objeto de salida (**Exit Object**) de manera automática usando **reflection**.  

Es una Librería de enlace de dos objetos. Mappea automaticamente propiedades de objetos y resuelve diferencias de tipos inteligentemente. 

Su objetivo es facilitar la conversión entre clases con propiedades similares (por ejemplo: **DTOs, ViewModels, Entities, etc.**) sin necesidad de escribir código repetitivo de asignación.

---

## ✨ Características

- Mapeo automático de propiedades con el mismo nombre (case-insensitive).  
- Conversión automática entre:
  - `string ↔ int`
  - `string ↔ bool`
  - `bool ↔ int`
- Control de seguridad para **propiedades nulas**.  
- Posibilidad de **omitir propiedades específicas** en el mapeo.  
- Excepciones claras y personalizadas (`LinkerException`).  
- ✅ **El objeto de salida se modifica directamente por referencia, por lo que no es necesario reasignar el resultado**.  

---

## 📦 Instalación

Si la librería está publicada en **NuGet**, puedes instalarla con:

```bash
dotnet add package MyLinker
```

Si no, basta con incluir el proyecto o el archivo `.cs` en tu solución.

---

## 🚀 Uso básico

### Ejemplo 1: Mapeo simple entre objetos

```csharp
using MyLinker.Linker;

var userDto = new UserDTO
{
    Id = "123",
    Name = "Juan",
    IsActive = "true"
};

var user = new User();

Linker<UserDTO, User>.MapObjects(userDto, user);

// No es necesario reasignar `user = ...`, ya que se edita la referencia.
Console.WriteLine(user.Id);       // 123 (int convertido desde string)
Console.WriteLine(user.Name);     // "Juan"
Console.WriteLine(user.IsActive); // true (bool convertido desde string)
```

### Ejemplo 2: Omitir propiedades en el mapeo

```csharp
var userDto = new UserDTO
{
    Id = "123",
    Name = "Juan",
    IsActive = "true"
};

var user = new User { Name = "Pedro" };

Linker<UserDTO, User>.MapObjects(userDto, user, propertyOmit: new[] { "Name" });

Console.WriteLine(user.Name); // "Pedro" (no fue reemplazado)
```

### Ejemplo 3: Controlar nulos

```csharp
var dto = new ProductDTO
{
    Name = null,
    Price = "150"
};

var product = new Product { Name = "Default" };

// nullPropertySafety = true (valor por defecto) → no sobrescribe valores con null
Linker<ProductDTO, Product>.MapObjects(dto, product);

Console.WriteLine(product.Name);  // "Default"
Console.WriteLine(product.Price); // 150
```

---

## ⚙️ Firma del método principal

```csharp
public static EX MapObjects(
    EN entryObject, 
    EX exitObject, 
    bool nullPropertySafety = true, 
    params string[]? propertyOmit
)
```

- **entryObject** → Objeto de entrada (fuente de datos).  
- **exitObject** → Objeto de salida (destino).  
- **nullPropertySafety** → Si es `true`, evita sobrescribir propiedades con valores `null`.  
- **propertyOmit** → Lista de nombres de propiedades que se deben omitir durante el mapeo.  

---

## ⚠️ Excepciones

- `ArgumentNullException` → Cuando `entryObject` o `exitObject` son nulos.  
- `LinkerException` → Cuando ocurre un error durante el mapeo interno.  

---

## 🛠️ Casos de uso comunes

- Mapear **DTOs ↔ Entidades** en aplicaciones con capas separadas.  
- Convertir entre **ViewModels y Models** en aplicaciones MVVM.  
- Transformar resultados de **APIs externas** hacia modelos internos.  

---

## 📐 Diagrama UML

```text
+----------------------+
|      Linker<EN,EX>   |
+----------------------+
| + MapObjects(        |
|   entryObject: EN,   |
|   exitObject: EX,    |
|   nullPropertySafety: bool, |
|   propertyOmit: string[] ) : EX |
+----------------------+
| - BaseMapObjects(...) : EX     |
+----------------------+
```

- Clase **genérica** que recibe dos tipos:  
  - `EN` → Tipo del objeto de entrada  
  - `EX` → Tipo del objeto de salida  
- Método público `MapObjects` que expone el mapeo.  
- Método privado `BaseMapObjects` que contiene la lógica interna.  

---

## 📜 Licencia

Este proyecto está bajo la licencia **MIT**. Puedes usarlo libremente en proyectos personales o comerciales.  
