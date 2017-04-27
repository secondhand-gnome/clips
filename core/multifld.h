   /*******************************************************/
   /*      "C" Language Integrated Production System      */
   /*                                                     */
   /*             CLIPS Version 6.40  11/01/16            */
   /*                                                     */
   /*                MULTIFIELD_TYPE HEADER FILE               */
   /*******************************************************/

/*************************************************************/
/* Purpose: Routines for creating and manipulating           */
/*   multifield values.                                      */
/*                                                           */
/* Principal Programmer(s):                                  */
/*      Gary D. Riley                                        */
/*                                                           */
/* Contributing Programmer(s):                               */
/*                                                           */
/* Revision History:                                         */
/*                                                           */
/*      6.24: Renamed BOOLEAN macro type to intBool.         */
/*                                                           */
/*            Corrected code to remove compiler warnings.    */
/*                                                           */
/*            Moved ImplodeMultifield from multifun.c.       */
/*                                                           */
/*      6.30: Changed integer type/precision.                */
/*                                                           */
/*            Changed garbage collection algorithm.          */
/*                                                           */
/*            Used DataObjectToString instead of             */
/*            ValueToString in implode$ to handle            */
/*            print representation of external addresses.    */
/*                                                           */
/*            Added const qualifiers to remove C++           */
/*            deprecation warnings.                          */
/*                                                           */
/*            Converted API macros to function calls.        */
/*                                                           */
/*            Fixed issue with StoreInMultifield when        */
/*            asserting void values in implied deftemplate   */
/*            facts.                                         */
/*                                                           */
/*      6.40: Refactored code to reduce header dependencies  */
/*            in sysdep.c.                                   */
/*                                                           */
/*            Removed LOCALE definition.                     */
/*                                                           */
/*            Pragma once and other inclusion changes.       */
/*                                                           */
/*            Added support for booleans with <stdbool.h>.   */
/*                                                           */
/*            Removed use of void pointers for specific      */
/*            data structures.                               */
/*                                                           */
/*            ALLOW_ENVIRONMENT_GLOBALS no longer supported. */
/*                                                           */
/*            UDF redesign.                                  */
/*                                                           */
/*************************************************************/

#ifndef _H_multifld

#pragma once

#define _H_multifld

#include "entities.h"

typedef struct multifieldBuilder MultifieldBuilder;

struct multifieldBuilder
  {
   Environment *mbEnv;
   CLIPSValue *contents;
   size_t bufferReset;
   size_t length;
   size_t bufferMaximum;
  };

   Multifield                    *CreateUnmanagedMultifield(Environment *,unsigned long);
   void                           ReturnMultifield(Environment *,Multifield *);
   void                           IncrementMultifieldReferenceCount(Environment *,Multifield *);
   void                           DecrementMultifieldReferenceCount(Environment *,Multifield *);
   void                           IncrementCLIPSValueMultifieldReferenceCount(Environment *,Multifield *);
   void                           DecrementCLIPSValueMultifieldReferenceCount(Environment *,Multifield *);
   Multifield                    *StringToMultifield(Environment *,const char *);
   Multifield                    *CreateMultifield(Environment *,size_t);
   void                           AddToMultifieldList(Environment *,Multifield *);
   void                           FlushMultifields(Environment *);
   void                           DuplicateMultifield(Environment *,UDFValue *,UDFValue *);
   void                           PrintMultifield(Environment *,const char *,Multifield *);
   void                           PrintMultifieldDriver(Environment *,const char *,Multifield *,size_t,size_t,bool);
   bool                           MultifieldDOsEqual(UDFValue *,UDFValue *);
   void                           StoreInMultifield(Environment *,UDFValue *,Expression *,bool);
   Multifield                    *CopyMultifield(Environment *,Multifield *);
   bool                           MultifieldsEqual(Multifield *,Multifield *);
   Multifield                    *DOToMultifield(Environment *,UDFValue *);
   unsigned long                  HashMultifield(Multifield *,unsigned long);
   Multifield                    *GetMultifieldList(Environment *);
   CLIPSLexeme                   *ImplodeMultifield(Environment *,UDFValue *);
   void                           EphemerateMultifield(Environment *,Multifield *);
   Multifield                    *ArrayToMultifield(Environment *,CLIPSValue *,unsigned long);
   void                           NormalizeMultifield(Environment *,UDFValue *);
   void                           CLIPSToUDFValue(CLIPSValue *,UDFValue *);
   MultifieldBuilder             *CreateMultifieldBuilder(Environment *,size_t);
   void                           MBReset(MultifieldBuilder *);
   void                           MBDispose(MultifieldBuilder *);
   void                           MBAppend(MultifieldBuilder *theMB,CLIPSValue *);
   Multifield                    *MBCreate(MultifieldBuilder *);
   Multifield                    *EmptyMultifield(Environment *);
   void                           MBAppendCLIPSInteger(MultifieldBuilder *,CLIPSInteger *);
   void                           MBAppendInt(MultifieldBuilder *,int);
   void                           MBAppendLong(MultifieldBuilder *,long);
   void                           MBAppendLongLong(MultifieldBuilder *,long long);
   void                           MBAppendCLIPSFloat(MultifieldBuilder *,CLIPSFloat *);
   void                           MBAppendFloat(MultifieldBuilder *,float);
   void                           MBAppendDouble(MultifieldBuilder *,double);
   void                           MBAppendCLIPSLexeme(MultifieldBuilder *,CLIPSLexeme *);
   void                           MBAppendSymbol(MultifieldBuilder *,const char *);
   void                           MBAppendString(MultifieldBuilder *,const char *);
   void                           MBAppendInstanceName(MultifieldBuilder *,const char *);
   void                           MBAppendCLIPSExternalAddress(MultifieldBuilder *,CLIPSExternalAddress *);
   void                           MBAppendFact(MultifieldBuilder *,Fact *);
   void                           MBAppendInstance(MultifieldBuilder *,Instance *);
   void                           MBAppendMultifield(MultifieldBuilder *,Multifield *);
   void                           MBAppendUDFValue(MultifieldBuilder *theMB,UDFValue *);

#endif /* _H_multifld */




