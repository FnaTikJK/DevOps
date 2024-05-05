import LoadingOutlined from "@ant-design/icons/lib/icons/LoadingOutlined";
import React from "react";

type Props = {
    withoutText?: boolean,
}

export const Loading = (props: Props) => {
    return (
        <>
            <LoadingOutlined size={500}/>
            {!props.withoutText && <p>Loading...</p>}
        </>
    )
}