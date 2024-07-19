using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Storage;

namespace ProyectoModulo.Services;

public class ServicioFirebase
{
    private FirebaseAuthClient CrearCliente(string apiKey)
    {
        try
        {
            //var auth = new FirebaseAuthProvider(new FirebaseConfig(config["api_key"]));
            var configAuth = new FirebaseAuthConfig()
            {
                ApiKey = apiKey,
                AuthDomain = "tiendaonline-79048.web.app",
                Providers = new FirebaseAuthProvider[]
                {
                    //new GoogleProvider().AddScopes("email"),
                    new EmailProvider()
                }
            };

            var cliente = new FirebaseAuthClient(configAuth);

            return cliente;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }

    }

    private async Task<string?> AutenticarAsync(FirebaseAuthClient cliente, string correo, string clave)
    {
        var credencialesUsuario = await cliente.SignInWithEmailAndPasswordAsync(correo, clave);
        var usuario = credencialesUsuario.User;

        return await usuario.GetIdTokenAsync();
    }

    public async Task<string> SubirImagen(Stream streamArchivo, string carpetaDestino, string nombreArchivo)
    {
        try
        {
            var consulta = _repositorie.Consultar(c => c.Recurso.Equals("FireBase_Storage"));

            Dictionary<string, string> config = consulta.ToDictionary(keySelector: c => c.Propiedad, elementSelector: c => c.Valor);

            FirebaseAuthClient cliente = CrearCliente(config["api_key"]);

            var firebaseToken = await AutenticarAsync(cliente, config["email"], config["clave"]);

            var tokenCancelacion = new CancellationTokenSource();

            var storage = new FirebaseStorage(
                config["ruta"],
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(firebaseToken),
                    ThrowOnCancel = true
                }
            ).Child(carpetaDestino)
            .Child(nombreArchivo)
            .PutAsync(streamArchivo, tokenCancelacion.Token);

            storage.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

            var urlImagen = await storage;
            return urlImagen;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return string.Empty;
        }
    }

    public async Task<bool> EliminarImagen(string carpetaDestino, string nombreArchivo)
    {
        try
        {

            var consulta = _repositorie.Consultar(c => c.Recurso.Equals("FireBase_Storage"));

            Dictionary<string, string> config = consulta.ToDictionary(keySelector: c => c.Propiedad, elementSelector: c => c.Valor);

            var cliente = CrearCliente(config["api_key"]);
            var firebaseToken = await AutenticarAsync(cliente, config["email"], config["clave"]);

            var storage = new FirebaseStorage(
                config["ruta"],
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(firebaseToken),
                    ThrowOnCancel = true
                }
            ).Child(carpetaDestino)
            .Child(nombreArchivo);

            await storage.DeleteAsync();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}
