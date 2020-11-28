import * as React from 'react';
import { Formik, Form, Field, FieldArray, useField, FieldAttributes, FormikHelpers } from 'formik';
import { Button, Card, CardActions, CardContent, CardHeader, FormControl, FormHelperText, Grid, IconButton, InputLabel, makeStyles, MenuItem, Paper, Snackbar, Tooltip, Typography } from '@material-ui/core';
import {ApiClientFactory} from '../helpers/api/ApiClientFactory';
import { CreateToDoItemCommand } from '../helpers/api/Resources';
import { useState } from 'react';
import { useParams } from 'react-router-dom';
import SaveIcon from '@material-ui/icons/Save';
import AddIcon from '@material-ui/icons/Add';
import DeleteIcon from '@material-ui/icons/Delete';
import TextField from '@material-ui/core/TextField';
import Select from '@material-ui/core/Select';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Checkbox from '@material-ui/core/Checkbox';
import * as Yup from 'yup';
import { useHistory } from 'react-router';
import TextFieldWithFormikValidation from '../components/TextFieldWithFormikValidation';
import Alert from '@material-ui/lab/Alert/Alert';
import LoadingProgress from '../components/LoadingProgress';
import camelcaseKeys from 'camelcase-keys';

const useStyles = makeStyles((theme) => ({
  paper: {
    padding: theme.spacing(3),
  },
  buttons: {
    display: 'flex',
    justifyContent: 'flex-end',
  },
  button: {
    margin: theme.spacing(1),
  }
}));

type ToDoCreateProps = {

}

const ToDoCreate : React.FC<ToDoCreateProps> = (props) => {
  const classes = useStyles();
  let history = useHistory();

  const [createCommand, setCreateCommand] = useState<CreateToDoItemCommand | undefined>(undefined);
  // UX states
  const [isJustSaved, setIsJustSaved] = useState<boolean>(false);
  const [hasServerError, setHasServerError] = useState<boolean>(false);
  const [serverErrorMessage, setServerErrorMessage] = useState<string>("");

  const client = ApiClientFactory.GetToDoItemClient();
  
  // add this method instead of using initial state in useState, 
  // so we can reuse this page later for edit purpose
  const loadCreateCommand = () => {
    let initialCommand = {
        title: "",
        category: ""
    }
    setCreateCommand(initialCommand);
  }

  // get createCommand on page load
  React.useEffect(()=>{
    loadCreateCommand()
  }, []); 

  const validationSchema = Yup.object({
    title: Yup.string().required("Title is required"),
    category: Yup.string().required("Category is required")
  });

  const handleCancel = (event: React.MouseEvent<HTMLButtonElement, MouseEvent>) : void => {
    history.push("/todolist");
  }

  const handleSubmit = (data : CreateToDoItemCommand, formikHelpers: FormikHelpers<any>) => {
    formikHelpers.setSubmitting(true);
    client.create(data)
          .then((response) => { 
            setIsJustSaved(true);
          })
          .catch((error) => {
            console.log(error);
            // General error message
            setHasServerError(true);
            setServerErrorMessage(error.title);
            // Field-specific errors from server side validation. 
            if(error.errors)
            {
              // Formik errors use camelcase for key values
              formikHelpers.setErrors(camelcaseKeys(error.errors));
            }
          })
          .finally(() => {
            formikHelpers.setSubmitting(false);
          });
  }
  
  const clearErrorState = () => 
  {
    setHasServerError(false);
    setServerErrorMessage("");
  }

  const renderCreateForm = (data: CreateToDoItemCommand) => {
    return (
      <Formik
          enableReinitialize
          initialValues={data}
          validationSchema={validationSchema}
          validateOnBlur={true}
          validateOnChange={true}
          onSubmit={handleSubmit} >
          { ({ values, errors, isSubmitting, setTouched }) => {
            return (
              <Form id="createForm">
                <Grid container spacing={2} >
                  <Grid item xs={12} sm={12}>
                    <TextFieldWithFormikValidation as={TextField} name="title" value={values.title} type="input" placeholder="title" label="Title *" fullWidth />
                  </Grid>
                  <Grid item xs={12} sm={12}>
                    <TextFieldWithFormikValidation as={TextField} name="category" value={values.category} type="input" placeholder="category" label="Category *" fullWidth />
                  </Grid>
                </Grid>
                <div className={classes.buttons}>
                  <Button
                      variant="contained"
                      color="default"
                      onClick={handleCancel}
                      className={classes.button}
                      >
                      Cancel
                  </Button>
                  <Button
                      type="submit"
                      variant="contained"
                      color="primary"
                      // onClick={() => {}}
                      className={classes.button}
                      disabled={isSubmitting}
                      >
                      Save
                  </Button>
                  <Snackbar open={isJustSaved} autoHideDuration={6000} onClose={() => setIsJustSaved(false)}>
                    <Alert onClose={() => setIsJustSaved(false)} severity="success">
                      Record is successfully saved.
                    </Alert>
                  </Snackbar>
                  <Snackbar open={hasServerError} autoHideDuration={6000} onClose={() => clearErrorState()}>
                    <Alert onClose={() => clearErrorState()} severity="error">
                      Error: {serverErrorMessage}
                    </Alert>
                  </Snackbar>
                </div>
                <Typography variant="h6">State</Typography>
                <pre>{JSON.stringify(values, null, 2)}</pre>
                <Typography variant="h6">Error</Typography>
                <pre>{JSON.stringify(errors, null, 2)}</pre>
              </Form>
            );
          }}
      </Formik>   
    );
  }

  return (
    <>
      <Paper className={classes.paper}>
        <LoadingProgress isLoading={createCommand === undefined}>
          {renderCreateForm(createCommand!)}
        </LoadingProgress>
      </Paper>
    </>
  );
}

export default ToDoCreate;