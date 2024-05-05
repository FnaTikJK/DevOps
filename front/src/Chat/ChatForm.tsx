import {User} from "../App";
import UserOutlined from "@ant-design/icons/lib/icons/UserOutlined";
import {Button} from "antd";
import {LogoutOutlined} from "@ant-design/icons";
import { Chat } from "./Chat";
import './ChatForm.css';


type Props = {
    user: User,
    onLogout: () => void,
    onPushErr: (message: string) => void,
}

export const ChatForm = (props: Props) => {
    return (
        <>
            <div className={"header"}>
                <span className={"account"}>
                    <UserOutlined />
                    <span>{props.user.login}</span>
                </span>
                <Button onClick={props.onLogout} className={"logout"}>
                    <LogoutOutlined />
                </Button>
            </div>
            <Chat user={props.user} onPushErr={props.onPushErr}/>
        </>
    )
}
