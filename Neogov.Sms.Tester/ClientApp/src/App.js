import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Messages from './components/Messages';
import LiveMessages from './components/LiveMessages';
import SendMessage from './components/SendMessage';

const App = props => {

    return (
        <Layout>
            <Route exact path='/' component={Home} />
            <Route path='/live' component={LiveMessages} />
            <Route path='/messages' component={Messages} />
            <Route path='/send' component={SendMessage} />
        </Layout>
    )
}

export default App
