import {api} from "../api";
import {Button} from "antd";
import {FileFilled} from "@ant-design/icons";

type Props = {
    id: string,
    name: string,
}

export const DownloadFile = (props: Props) => {
    const onDownload = async () => {
        try {
            const response = await api.statics.download(props.id!);
            const blob = response.data;
            const link = document.createElement('a');
            link.href = URL.createObjectURL(blob);
            link.setAttribute('download', props.name!);
            document.body.appendChild(link);
            link.click();
            link.remove();
        } catch (e) {
            console.log(e);
        }
    }

    return (
        <>
            <Button onClick={onDownload}>
                <FileFilled />
                {props.name}
            </Button>
        </>
    )
}