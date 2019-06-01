using Renci.SshNet;

public class Ssh 
{
    public string ExecuteCommand(){
        //192.168.1.108
        SshClient sshclient = new SshClient("recalbox", "root", "recalboxroot");    
        sshclient.Connect();
        SshCommand sc= sshclient .CreateCommand("/etc/init.d/S31emulationstation stop");
        sc.Execute();
        return sc.Result;
    }
}