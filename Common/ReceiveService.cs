using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IReceiveService
    {
        [OperationContract(IsOneWay = true)]
        void SendPackage(PackageDto dto);
    }

    [DataContract]
    public class PackageDto
    {
        [DataMember]
        public int Value { get; set; }
        [DataMember]
        public ulong PackageNumber { get; set; }
        [DataMember]
        public DateTime PackageDate { get; set; }
    }

    public class ReceiveService : IReceiveService
    {   
        public void SendPackage(PackageDto dto)
        {
            try
            {
                Storage.AddPackage(dto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            
        }
    }

}
