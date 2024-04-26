import React, {useEffect, useRef, useState} from 'react';
import logo from './logo.svg';
import './App.css';
import {api} from "./api";
import LoadingOutlined from '@ant-design/icons/lib/icons/LoadingOutlined';
import {LoginForm} from "./LoginForm";
import {Alert, message} from "antd";

export type User = {
    id: string,
    token: string,
};

type PageState = "loading" | "login" | "chat";

function App() {
    const [pageState, setPageState] = useState<PageState>("loading");
    const [user, setUser] = useState<User | null>(null);
    const [messageApi, contextHolder] = message.useMessage();


    useEffect(() => {
        (async () => {
            const authInfo = await api.acc.my();
            if (authInfo.status === 401 || authInfo.status === 403) {
                setPageState("login");
                return;
            }

            setUser({
                id: authInfo.data.id,
                token: authInfo.data.jwtToken,
            });
            setPageState("login");
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
            {/*<header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.tsx</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>*/}
        </div>
    );
}

const Loading = () => {
    return (
        <>
            <LoadingOutlined size={500}/>
            <p>Loading...</p>
        </>
    )
}

export default App;
