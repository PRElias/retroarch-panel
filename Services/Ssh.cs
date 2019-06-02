using Renci.SshNet;

public class Ssh 
{
    public string ExecuteCommand(string command){
        //192.168.1.108
        SshClient sshclient = new SshClient("recalbox", "root", "recalboxroot");    
        sshclient.Connect();
        SshCommand sc= sshclient.CreateCommand(command);
        // sshclient.CreateCommand("~/etc/init.d/S31emulationstation stop");
        sc.Execute();
        sshclient.Disconnect();
        sshclient.Dispose();
        if (sc.Error != string.Empty){
            return sc.Error;
        }
        else
        {
            return sc.Result;
        }
    }
}