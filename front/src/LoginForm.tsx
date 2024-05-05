import LockOutlined from "@ant-design/icons/lib/icons/LockOutlined";
import UserOutlined from "@ant-design/icons/lib/icons/UserOutlined";
import {Button, Form, Input, Radio } from "antd";
import {User} from "./App";
import {useState} from "react";
import {api} from "./api";
import './LoginForm.css';

type Props = {
    onSetUser: (user: User) => void,
    onPushError: (message: string) => void,
}

export const LoginForm = (props: Props) => {
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [isReg, setIsReg] = useState<boolean>(false);

    const reqRules = [{ required: true, message: 'Неправильный формат', pattern: new RegExp("^[0-9a-zA-Z!]*$")}];

    const onAuth = async (values: {Login: string, Password: string}) => {
        setIsLoading(true);

        if (values.Login.indexOf(" ") > -1 || values.Password.indexOf(" ") > -1
            || values.Login.length === 0 || values.Password.length === 0) {
            props.onPushError("Поля не должны содержать пробелы");
            setIsLoading(false);
        }

        try {
            let response = isReg
                ? await api.acc.reg(values.Login, values.Password)
                : await api.acc.auth(values.Login, values.Password);

            props.onSetUser({id: response.data.id, token: response.data.jwtToken, login: response.data.login});
        } catch (e) {
            // @ts-ignore
            props.onPushError( `${e.response.status}\n${e.response.data}`);
            setIsLoading(false)
        }
    }

    return (
        <div className={"main"}>
            <Form name="normal_login" initialValues={{ remember: true }} onFinish={onAuth} disabled={isLoading}>
                <Form.Item>
                    <Radio.Group defaultValue="login" onChange={(e) => setIsReg(e.target.value === "reg")}>
                        <Radio.Button value={"login"}>Вход</Radio.Button>
                        <Radio.Button value={"reg"}>Регистрация</Radio.Button>
                    </Radio.Group>
                </Form.Item>
                <Form.Item name="Login" rules={reqRules}>
                    <Input
                        prefix={<UserOutlined />}
                        placeholder="Логин"
                    />
                </Form.Item>
                <Form.Item name="Password" rules={reqRules}>
                    <Input
                        prefix={<LockOutlined />}
                        type="password"
                        placeholder="Пароль"
                    />
                </Form.Item>

                <Form.Item>
                    <Button type="primary" htmlType="submit" className="login-form-button" loading={isLoading}>
                        {isReg ? "Зарегистрироваться" : "Войти"}
                    </Button>
                </Form.Item>
            </Form>
        </div>
    );
}