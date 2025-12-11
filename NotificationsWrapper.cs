using OsNotifications;
using System;
namespace managementProj;
public static class NotificationsWrapper{

  public static void sendNotification(string text){
    try{
      Notifications.ShowNotification("management", text);
    }catch(Exception ex){

    }

  }



}
