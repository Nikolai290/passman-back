using System.Threading.Tasks;

namespace passman_back.Business.Interfaces.Services {
    public interface ICrypter {
        string Decrypt(string p);
        string Encrypt(string p);
    }
}
