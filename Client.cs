using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
// класс добавлен для демонстрации
//Урок 5.Базы данных: Entity framework, code first / db first
// Реализуйте тип сообщений List, при котором клиент будет получать все непрочитанные сообщения с сервера.
namespace DBtest
{
    internal class Client
    {
        public static async Task SendMsg(string name)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5430);
            UdpClient udpClient = new UdpClient();
            while (true)
            {
                Console.WriteLine("Введите имя получателя:");

                string toName = Console.ReadLine();
                if (String.IsNullOrEmpty(toName))
                {
                    Console.WriteLine("Вы не ввели имя пользователя.");
                    continue;
                }
                Console.WriteLine("Введите сообщение или <Exit> для выхода из Client.cs (внутри while (true):");

                string text = Console.ReadLine();

                MessageUDP mssg4 = new MessageUDP();
                mssg4.ToName = toName;
                mssg4.FromName = name;
                mssg4.Command = Command.Register;

                string responseMessgJs = mssg4.ToJson(); //было до шаблона Прототип
                                                     
                byte[] responseData = Encoding.UTF8.GetBytes(responseMessgJs);
                await udpClient.SendAsync(responseData, responseData.Length, ep);

                byte[] answerData = udpClient.Receive(ref ep);

                string answerMsgJs = Encoding.UTF8.GetString(answerData);
                MessageUDP answerMsg = MessageUDP.FromJson(answerMsgJs);
                Console.WriteLine(answerMsg.ToString());
            }
        }
    }
}
