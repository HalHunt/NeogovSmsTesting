import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Messages from './components/Messages';
import LiveMessages from './components/LiveMessages';

const App = props => {

    return (
        <Layout>
            <Route exact path='/' component={Home} />
            <Route path='/live' component={LiveMessages} />
            <Route path='/messages' component={Messages} />
        </Layout>
    )
}

export default App
