import React from 'react';
import { Formik, Form, Field, FieldArray, useField, FieldAttributes } from 'formik';
import TextField from '@material-ui/core/TextField';

type TextFieldWithFormikValidationProps = {label: string, fullWidth: boolean} & FieldAttributes<{}>;

const TextFieldWithFormikValidation : React.FC<TextFieldWithFormikValidationProps> = ({placeholder, label, fullWidth, required, ...props }) => {
  const [field, meta] = useField<{}>(props);
  const errorText = meta.error && meta.touched ? meta.error : '';

  return (
    <>
      <TextField {...field} helperText={errorText} error={!!errorText} placeholder={placeholder} label={label} fullWidth={fullWidth} required={required}></TextField>
    </>
  );
  
}

export default TextFieldWithFormikValidation;