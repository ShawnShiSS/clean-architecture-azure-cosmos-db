import * as React from 'react';
import {ApiClientFactory} from '../../helpers/api/ApiClientFactory';

// required by Material Table, see documentation: https://material-table.com/#/docs/install
import MaterialTable, { Icons, Query, QueryResult } from 'material-table';
import { forwardRef } from 'react';
import AddBox from '@material-ui/icons/AddBox';
import ArrowUpward from '@material-ui/icons/ArrowUpward';
import Check from '@material-ui/icons/Check';
import ChevronLeft from '@material-ui/icons/ChevronLeft';
import ChevronRight from '@material-ui/icons/ChevronRight';
import Clear from '@material-ui/icons/Clear';
import DeleteOutline from '@material-ui/icons/DeleteOutline';
import Edit from '@material-ui/icons/Edit';
import FilterList from '@material-ui/icons/FilterList';
import FirstPage from '@material-ui/icons/FirstPage';
import LastPage from '@material-ui/icons/LastPage';
import Remove from '@material-ui/icons/Remove';
import SaveAlt from '@material-ui/icons/SaveAlt';
import Search from '@material-ui/icons/Search';
import ViewColumn from '@material-ui/icons/ViewColumn';

const tableIcons : Icons = {
  Add: forwardRef((props, ref) => <AddBox {...props} ref={ref} />),
  Check: forwardRef((props, ref) => <Check {...props} ref={ref} />),
  Clear: forwardRef((props, ref) => <Clear {...props} ref={ref} />),
  Delete: forwardRef((props, ref) => <DeleteOutline {...props} ref={ref} />),
  DetailPanel: forwardRef((props, ref) => <ChevronRight {...props} ref={ref} />),
  Edit: forwardRef((props, ref) => <Edit {...props} ref={ref} />),
  Export: forwardRef((props, ref) => <SaveAlt {...props} ref={ref} />),
  Filter: forwardRef((props, ref) => <FilterList {...props} ref={ref} />),
  FirstPage: forwardRef((props, ref) => <FirstPage {...props} ref={ref} />),
  LastPage: forwardRef((props, ref) => <LastPage {...props} ref={ref} />),
  NextPage: forwardRef((props, ref) => <ChevronRight {...props} ref={ref} />),
  PreviousPage: forwardRef((props, ref) => <ChevronLeft {...props} ref={ref} />),
  ResetSearch: forwardRef((props, ref) => <Clear {...props} ref={ref} />),
  Search: forwardRef((props, ref) => <Search {...props} ref={ref} />),
  SortArrow: forwardRef((props, ref) => <ArrowUpward {...props} ref={ref} />),
  ThirdStateCheck: forwardRef((props, ref) => <Remove {...props} ref={ref} />),
  ViewColumn: forwardRef((props, ref) => <ViewColumn {...props} ref={ref} />)
};

const ToDoDataTable : React.FC = () => {

  const client = ApiClientFactory.GetToDoItemClient();
  const columns = [
    { title: 'Category', field: 'category' },
    { title: 'Title', field: 'title' }
  ];

  const loadRemoateData = (query: Query<object>) : Promise<QueryResult<object>> => 
  {
    return (
      new Promise<QueryResult<object>>((resolve, reject) => {
        const sortColumn : string = query.orderBy !== undefined ? String(query.orderBy.field) : "";          
        const sortDirection = String(query.orderDirection) === "" || String(query.orderDirection) === "asc" ? "Ascending" : "Descending";
        client.search({
          "start": query.page*query.pageSize,
          "pageSize": query.pageSize,
          "sortColumn": sortColumn,
          "sortDirection": sortDirection,
          "titleFilter": query.search
        })
        .then(response => {
          resolve({
            data: response.result.data,
            page: response.result.page, // current page
            totalCount: response.result.totalRecords!
          });
        })
        .catch(() => {
          reject("Error occurred while retrieving date.");
        });
      })

    );
  }

  return (
    <MaterialTable
        icons={tableIcons}
        title="Todo Items"
        columns={columns}
        data={loadRemoateData}        
        actions={[
          {
            icon: () => <Edit />,
            tooltip: 'Edit',
            onClick: (event, data) => console.log(data)
          }
        ]}
        options={{
          actionsColumnIndex: -1,
          exportButton: true,
          headerStyle: {
            fontWeight: "bold"
          }
        }}
      />
  )
}

export default ToDoDataTable;