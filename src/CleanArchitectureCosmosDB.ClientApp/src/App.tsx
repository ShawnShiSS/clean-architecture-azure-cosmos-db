import React from 'react';
import { Link as RouterLink, Switch, Route, Redirect } from 'react-router-dom';
import './App.css';
import DashboardLayout from './layouts/DashboardLayout'
import Dashboard from './views/Dashboard'

function App() {
  return (
    <>
      <DashboardLayout>
        <Switch>
          <Redirect exact from="/" to="/dashboard" />
          <Route path="/dashboard" exact={true} component={Dashboard} />
          
        </Switch>
        
      </DashboardLayout>

    </>
  );
}

export default App;
