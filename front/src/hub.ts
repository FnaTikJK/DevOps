import * as signalR from "@microsoft/signalr";

type hubProps = {
    token: string
    onCreateOrUpdate: (messageId: string) => void,
    onDelete: (messageId: string) => void,
    onPushErr: (message: string) => void,
}

enum NotifyType {
    CreateOrUpdate = 0,
    Delete = 1,
}

export const getHub = (props: hubProps) => {
    const domen = process.env.NODE_ENV === "development"
        ? "https://localhost:44357"
        : "";
    const url = domen + "/Hubs/Messages";
    let connection: signalR.HubConnection;

    connection = new signalR.HubConnectionBuilder()
        .withUrl(url, {
            withCredentials: true,
            accessTokenFactory: () => props.token,
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets,
        })
        .withAutomaticReconnect()
        .build();

    connection.on("Notify", obj => {
        if (obj.type === NotifyType.CreateOrUpdate)
            props.onCreateOrUpdate(obj.messageId);
        else
            props.onDelete(obj.messageId);
    })

    connection.start()
        .catch(err => {
            props.onPushErr(err.message)
        });

    return connection;
}
