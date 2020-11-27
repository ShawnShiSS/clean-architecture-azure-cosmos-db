import React from "react";
import MuiAlert, { AlertProps } from '@material-ui/lab/Alert';

const Alert : React.FC<AlertProps>= (props) => {
    return (
        <MuiAlert elevation={6} variant="filled" {...props} />
    );
}

export default Alert;