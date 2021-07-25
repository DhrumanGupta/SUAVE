import "./stylesheets/Common.scss";
import {BrowserRouter, Switch, Route} from 'react-router-dom';
import Home from "./pages/Home";
import NotFound from "./pages/NotFound";
import Search from "./pages/Search";

function App() {
  return (
    <BrowserRouter>
      <Switch>
        <Route exact path={'/'} component={Home}/>
        <Route exact path={'/search'} component={Search}/>
        <Route component={NotFound}/>
      </Switch>
    </BrowserRouter>
  );
}

export default App;
