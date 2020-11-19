import React, { useState } from 'react';
import { createStyles, makeStyles, Theme } from '@material-ui/core/styles';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import ListItemSecondaryAction from '@material-ui/core/ListItemSecondaryAction';
import ListItemText from '@material-ui/core/ListItemText';
import Checkbox from '@material-ui/core/Checkbox';
import IconButton from '@material-ui/core/IconButton';
import CommentIcon from '@material-ui/icons/Comment';

// API 
import {ToDoItemModel} from '../helpers/api/Resources';
import {ApiClientFactory} from '../helpers/api/ApiClientFactory';


const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      width: '100%',
    //   maxWidth: 360,
      backgroundColor: theme.palette.background.paper,
    },
  }),
);

const TodoList : React.FC = () => {
  const classes = useStyles();
  const apiClient = ApiClientFactory.GetToDoItemClient();

  const [checked, setChecked] = React.useState([0]);
  const [todoList, setTodoList] = useState<ToDoItemModel[]>([]);
  const loadTodoList = () => {
    apiClient.getAll().then((response) => {
        setTodoList(response);
    });
  };
  
  React.useEffect(
    () => { 
        loadTodoList()
        console.log(todoList)
    }, 
    [] // providing empty array so that useEffect will only run once, as value of [] does not change.
  ); 


  const handleToggle = (value: number) => () => {
    const currentIndex = checked.indexOf(value);
    const newChecked = [...checked];

    if (currentIndex === -1) {
      newChecked.push(value);
    } else {
      newChecked.splice(currentIndex, 1);
    }

    setChecked(newChecked);
  };

  return (
    <List className={classes.root}>
      {todoList.map((value) => {
        const labelId = `checkbox-list-label-${value.id}`;

        return (
          <ListItem key={value.id} role={undefined} dense button onClick={()=>{}}>
            <ListItemIcon>
              <Checkbox
                edge="start"
                checked={!value.isCompleted}
                tabIndex={-1}
                disableRipple
                inputProps={{ 'aria-labelledby': labelId }}
              />
            </ListItemIcon>
            <ListItemText id={labelId} primary={`${value.category}`} />
            <ListItemText id={labelId} primary={`${value.title}`} />
            <ListItemSecondaryAction>
              <IconButton edge="end" aria-label="comments">
                <CommentIcon />
              </IconButton>
            </ListItemSecondaryAction>
          </ListItem>
        );
      })}
    </List>
  );
}

export default TodoList