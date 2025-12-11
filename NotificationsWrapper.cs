using OsNotifications;
using System;
namespace managementProj;
public static class NotificationsWrapper{

  public static void sendNotification(string title, string text){
    try{
      Notifications.ShowNotification(title, text);
    }catch(Exception ex){

    }

  }



}
