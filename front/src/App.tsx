import React, {useEffect, useState} from 'react';
import './App.css';
import {api} from "./api";
import {LoginForm} from "./LoginForm";
import {message} from "antd";
import {AxiosError} from "axios";
import { Loading } from './Loading';
import { ChatForm } from './Chat/ChatForm';

export type User = {
    id: string,
    token: string,
    login: string,
};

type PageState = "loading" | "login" | "chat";

function App() {
    const [pageState, setPageState] = useState<PageState>("loading");
    const [user, setUser] = useState<User | null>(null);
    const [messageApi, contextHolder] = message.useMessage();


    useEffect(() => {
        (async () => {
            try {
                const authInfo = await api.acc.my();
                setUser({
                    id: authInfo.data.id,
                    token: authInfo.data.jwtToken,
                    login: authInfo.data.login,
                });
                setPageState("chat");
            } catch (e){
                const axErr = e as AxiosError;
                if (axErr?.response?.status === 401)
                    setPageState("login");
                else{
                    pushErr(axErr.message);
                }
            }
        })()
    }, []);

    const pushErr = (message: string) => {
        messageApi.open({
            type: "error",
            content: message,
        })
    };

    return (
        <div className="App">
            {contextHolder}
            {pageState === "loading" && <Loading/>}
            {pageState === "login" && <LoginForm
                onSetUser={(user) => {
                    setUser(user);
                    setPageState("chat");
                }}
                onPushError={(err) => pushErr(err)}
            />}
            {pageState === "chat" && <ChatForm
                user={user as User}
                onLogout={() => {
                    setPageState("login");
                    setUser(null);
                    api.acc.logout();
                }}
                onPushErr={(err) => pushErr(err)}
            />}
        </div>
    );
}



export default App;
