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
    const [errors, setErrors] = useState<ErrorMes[]>([]);
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
            setPageState("chat");
        })()
    }, []);

    const pushErr = (err: ErrorMes) => {
        var newErrors = errors.slice();
        newErrors.push(err);
        setErrors(newErrors);
    };
    const removeErr = (err: ErrorMes) => {
        var newErrors = errors.slice();
        const ind = newErrors.findIndex((e, ind, arr) => e === err);
        newErrors.splice(ind);
        setErrors(newErrors);
    }

    return (
        <div className="App">
            {pageState === "loading" && <Loading/>}
            {pageState === "login" && <LoginForm
                onSetUser={(user) => {
                    setUser(user);
                    setPageState("chat");
                }}
                onPushError={(err) => pushErr(err)}
            />}
            <Errors errors={errors} onRemove={removeErr}/>
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

const Errors = (props: { errors: ErrorMes[], onRemove: (err: ErrorMes) => void }) => {
    const errors = props.errors;

    return (
        <div className={"ErrorContainer"}>
            {errors.map(err =>
                <Error
                    onClose={() => props.onRemove(err)}
                    errorMes={err}
                />)}
        </div>
    )
}

export type ErrorMes = {
    title: string, message: string
}

const Error = (props: { onClose: () => void, errorMes: ErrorMes }) => {
    return (
        <div className={"Error"}>
            <Alert
                message={props.errorMes.title}
                description={props.errorMes.message}
                type="error"
                closable
                onClose={props.onClose}
            />
        </div>
    )
}

export default App;
