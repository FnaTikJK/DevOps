import {User} from "../App";
import {useEffect, useRef, useState} from "react";
import {HubConnection} from "@microsoft/signalr";
import {api, Message} from "../api";
import {getHub} from "../hub";
import InfiniteScroll from "react-infinite-scroll-component";
import {List} from "antd";
import {DeleteOutlined, EditOutlined} from "@ant-design/icons";
import { TextArea } from "./TextArea";
import {DownloadFile} from "../Files/DownloadFile";

type ChatProps = {
    user: User,
    onPushErr: (message: string) => void,
}

export const Chat = (props: ChatProps) => {
    const hubRef = useRef<HubConnection | null>(null);
    const [messages, setMessages] = useState<Message[]>([]);
    const [editingMessage, setEditingMessage] = useState<Message | undefined>(undefined);

    useEffect(() => {(async () => {
        try {
            const newMessages = await api.messages.getAll();
            setMessages((_) => [...newMessages.data]);
            hubRef.current ??= getHub({
                token: props.user.token,
                onDelete: onDeleteMessageView,
                onCreateOrUpdate: onCreateOrUpdate,
                onPushErr: props.onPushErr,
            });
        } catch(e) {
            // @ts-ignore
            props.onPushErr(e.message);
        }
    })()}, []);

    const onCreateOrUpdate = async (id: string) => {
        const newMessage = await api.messages.getById(id);
        setMessages((prev) => {
            const existed = prev.some(m => m.id === id);
            if (existed)
                return prev.map(m => m.id !== id ? m : newMessage.data)

            return [...prev, newMessage.data];
        });
    }

    const onDeleteMessageView = (id: string) => {
        setMessages((prev) => prev.filter(m => m.id !== id))
    }

    const onDeleteMessage = (id: string) => {
        setEditingMessage(undefined);
        api.messages.delete(id);
        onDeleteMessageView(id);
    }

    return (
        <div>
            <div id="scrollableDiv" className={"chat"}>
                <InfiniteScroll
                    dataLength={messages.length}
                    next={() => {}}
                    hasMore={false}
                    scrollableTarget="scrollableDiv"
                    loader={"Loading..."}
                >
                    <List
                        dataSource={messages}
                        renderItem={(message) => (
                            <List.Item key={message.id}>
                                <List.Item.Meta
                                    title={message.author.login}
                                    description={renderMessage(message)}
                                />
                                {message.author.id !== props.user.id
                                    ? null
                                    : (<>
                                        <EditOutlined onClick={() => setEditingMessage(message)}/>
                                        <DeleteOutlined onClick={() => onDeleteMessage(message.id)}/>
                                    </>)}
                            </List.Item>
                        )}
                    />
                </InfiniteScroll>
            </div>
            <div>
                <TextArea
                    message={editingMessage}
                    onCancel={() => setEditingMessage(undefined)}
                    onSend={() => setEditingMessage(undefined)}
                />
            </div>
        </div>
    )
}

const renderMessage = (message: Message) => (
    <>
        <div>{message.text}</div>
        <div>
            {message.files.map(f => (<DownloadFile id={f.id} name={f.fileName}/>))}
        </div>
    </>
)
