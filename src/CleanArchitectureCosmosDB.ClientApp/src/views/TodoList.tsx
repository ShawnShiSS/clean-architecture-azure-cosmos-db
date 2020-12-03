import React from 'react';
import { createStyles, makeStyles, Theme } from '@material-ui/core/styles';
import { Link as RouterLink } from 'react-router-dom';
import AddIcon from '@material-ui/icons/Add';
import ToDoDataTable from '../components/ToDo/ToDoDataTable';

// API 
import { Box, Button } from '@material-ui/core';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    boxroot: {
      width: '100%',
      backgroundColor: theme.palette.background.paper,
      padding: theme.spacing(2)
    },
    box: {
      display: "flex",
      justifyContent: "flex-end"
    },
    datatableRoot: {
      display: 'flex',
      height: '100%' 
    },
    datatable: { 
      flexGrow: 1 
    }
  }),
);

const TodoList : React.FC = () => {
  const classes = useStyles();
  
  return (
    <>
      <div className={classes.boxroot}>
        <Box className={classes.box}>
          <Button
            color="primary"
            variant="contained"
            component={RouterLink}
            to="/todolist/create"
            startIcon={<AddIcon />}
          >
            New
          </Button>

        </Box>
      </div>
      <div className={classes.datatableRoot}>
        <div className={classes.datatable}>
          <ToDoDataTable />
        </div>
      </div>
    </>
    
  );
}

export default TodoList