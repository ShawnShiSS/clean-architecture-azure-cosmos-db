import React from 'react';
import { Link as RouterLink, Switch, Route, Redirect } from 'react-router-dom';
import './App.css';
import DashboardLayout from './layouts/DashboardLayout'
import Dashboard from './views/Dashboard'
import TodoList from './views/TodoList'
import ToDoCreate from './views/ToDoCreate'

function App() {
  return (
    <>
      <DashboardLayout>
        <Switch>
          <Redirect exact from="/" to="/dashboard" />
          <Route path="/dashboard" exact={true} component={Dashboard} />
          <Route path="/todolist/create" exact={true} component={ToDoCreate} />
          <Route path="/todolist" exact={true} component={TodoList} />
          
        </Switch>
        
      </DashboardLayout>

    </>
  );
}

export default App;
