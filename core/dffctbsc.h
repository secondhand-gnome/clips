   /*******************************************************/
   /*      "C" Language Integrated Production System      */
   /*                                                     */
   /*             CLIPS Version 6.40  07/05/16            */
   /*                                                     */
   /*         DEFFACTS BASIC COMMANDS HEADER FILE         */
   /*******************************************************/

/*************************************************************/
/* Purpose: Implements core commands for the deffacts        */
/*   construct such as clear, reset, save, undeffacts,       */
/*   ppdeffacts, list-deffacts, and get-deffacts-list.       */
/*                                                           */
/* Principal Programmer(s):                                  */
/*      Gary D. Riley                                        */
/*                                                           */
/* Contributing Programmer(s):                               */
/*      Brian L. Dantes                                      */
/*                                                           */
/* Revision History:                                         */
/*                                                           */
/*      6.23: Corrected compilation errors for files         */
/*            generated by constructs-to-c. DR0861           */
/*                                                           */
/*      6.24: Renamed BOOLEAN macro type to intBool.         */
/*                                                           */
/*      6.30: Removed conditional code for unsupported       */
/*            compilers/operating systems (IBM_MCW,          */
/*            MAC_MCW, and IBM_TBC).                         */
/*                                                           */
/*            Added const qualifiers to remove C++           */
/*            deprecation warnings.                          */
/*                                                           */
/*            Converted API macros to function calls.        */
/*                                                           */
/*      6.40: Removed LOCALE definition.                     */
/*                                                           */
/*            Pragma once and other inclusion changes.       */
/*                                                           */
/*            Added support for booleans with <stdbool.h>.   */
/*                                                           */
/*************************************************************/

#ifndef _H_dffctbsc

#pragma once

#define _H_dffctbsc

#include "evaluatn.h"

   void                           DeffactsBasicCommands(void *);
   void                           UndeffactsCommand(void *);
   bool                           EnvUndeffacts(void *,void *);
   void                           GetDeffactsListFunction(void *,DATA_OBJECT_PTR);
   void                           EnvGetDeffactsList(void *,DATA_OBJECT_PTR,void *);
   void                          *DeffactsModuleFunction(void *);
   void                           PPDeffactsCommand(void *);
   int                            PPDeffacts(void *,const char *,const char *);
   void                           ListDeffactsCommand(void *);
   void                           EnvListDeffacts(void *,const char *,void *);

#if ALLOW_ENVIRONMENT_GLOBALS

   void                           GetDeffactsList(DATA_OBJECT_PTR,void *);
   bool                           Undeffacts(void *);
#if DEBUGGING_FUNCTIONS
   void                           ListDeffacts(const char *,void *);
#endif

#endif /* ALLOW_ENVIRONMENT_GLOBALS */

#endif /* _H_dffctbsc */

